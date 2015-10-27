#include <SoftPWM.h>

#define RED 0
#define GREEN 1
#define BLUE 2

#define NR_LEDS 6
#define NR_COLORS 3
#define NR_COMMANDS 21

#define HP_LED 5

#define MAX_PERIOD 3000 //ms
#define MIN_PERIOD 100  //ms

enum COMMAND
{
    SLIDER_MASTER = 0,
    PROGRAM,
    FADE,
    SLIDER_1,
    SLIDER_2,
    SLIDER_3,
    HP,
    SLIDER_MASTER_HP,
    PROGRAM_HP,
    FADE_HP,
    SLIDER_1_HP,
    BOOM,
    BOOM_HP,
    ZERO,
    ZERO_HP,
    RED_M,
    GREEN_M,
    BLUE_M,
    RED_M_HP,
    GREEN_M_HP,
    BLUE_M_HP
};

enum PROGRAMS_MAIN
{
    MANUAL = 0,
    SEMI_MAN_SEQ,
    SEMI_MAN_RAN,
    FIRE,
    PARTY
};

enum PROGRAMS_HP
{
    RED_HP = 0,
    GREEN_HP,
    BLUE_HP,
    AUTO_SEQ_HP,
    AUTO_RAN_HP
};

// Global variables
byte receivedMessage[2];
byte RGB_LEDS[NR_LEDS][NR_COLORS];
byte savedCommands[NR_COMMANDS];
int sliderAsPeriod[4];

byte outLedValue[NR_LEDS][NR_COLORS];
int outFadeValue[NR_LEDS][NR_COLORS];
bool outUpdateValue[NR_LEDS][NR_COLORS];
byte nr_ActiveLeds = NR_LEDS;
long currentTime = millis();
long previousTime[4];
int preProgram = -1;
int preProgram_HP = -1;
bool programSwitched = true;
bool programSwitched_HP = true;
int counter=0;

void setup() {
  
  //Init softPWM
  SoftPWMBegin();

  //Init serial
  Serial.begin(9600);  
  
  //Init led pins
  RGB_LEDS[0][0] = 2;
  RGB_LEDS[0][1] = 3;
  RGB_LEDS[0][2] = 4;
  
  RGB_LEDS[1][0] = 5;
  RGB_LEDS[1][1] = 6;
  RGB_LEDS[1][2] = 7;
  
  RGB_LEDS[2][0] = 8;
  RGB_LEDS[2][1] = 9;
  RGB_LEDS[2][2] = 17;
  
  RGB_LEDS[3][0] = 16;
  RGB_LEDS[3][1] = 15;
  RGB_LEDS[3][2] = 14;
  
  RGB_LEDS[4][0] = 13;
  RGB_LEDS[4][1] = 12;
  RGB_LEDS[4][2] = 11;
  
  RGB_LEDS[5][0] = 18;
  RGB_LEDS[5][1] = 10;
  RGB_LEDS[5][2] = 19;
  
  //Reset all soft pwm values
  for(byte i=0; i<NR_LEDS;i++){
    SoftPWMSet(RGB_LEDS[i][0], 0);
    SoftPWMSet(RGB_LEDS[i][1], 0);
    SoftPWMSet(RGB_LEDS[i][2], 0);
  }
  
  // init the fire simulator 
  initFire();
}

void loop() {
  
  // Check for input form serial
  if(Serial.available()>0){
    // read the message
    if(ReadSerial(receivedMessage)){
      // save command
      SaveCommand(receivedMessage);
    }
  }
  // check if program is switched
  if(preProgram != savedCommands[PROGRAM]){
    programSwitched = true;
    preProgram = savedCommands[PROGRAM];
  }else{
    programSwitched = false;
  }
  
  if(preProgram_HP != savedCommands[PROGRAM_HP]){
    programSwitched_HP = true;
    preProgram_HP = savedCommands[PROGRAM_HP];
  }else{
    programSwitched_HP = false;
  }
  
  // execute programs
  currentTime = millis();
  if(savedCommands[HP] == 0){
    // no highpower output is used
    nr_ActiveLeds = NR_LEDS;
  }else{
    // highpower is used sperate
    nr_ActiveLeds = NR_LEDS - 1;
    // execute high power program
    switch(savedCommands[PROGRAM_HP]){
      case RED_HP: _progHPRed(); break;
      case GREEN_HP: _progHPGreen(); break;
      case BLUE_HP: _progHPBlue(); break;
      case AUTO_SEQ_HP: _progSemiManHP(false); break;
      case AUTO_RAN_HP: _progSemiManHP(true);break;
      default: break; 
    }
  }
  // execute normal program
  switch(savedCommands[PROGRAM]){
    case MANUAL: _progManual(); break;
    case SEMI_MAN_SEQ:_progSemiMan(false);break;
    case SEMI_MAN_RAN:_progSemiMan(true);break;
    case FIRE: _progFire(); break;
    case PARTY: break;
    default: break; 
  }
  
  //over rule with masters
  for(byte l=0; l<nr_ActiveLeds;l++){
    if(savedCommands[RED_M]){
      outFadeValue[l][RED] = 0;
      outUpdateValue[l][RED] = true;
      outLedValue[l][RED] = 100;
    }
    if(savedCommands[GREEN_M]){
      outFadeValue[l][GREEN] = 0;
      outUpdateValue[l][GREEN] = true;
      outLedValue[l][GREEN] = 100;
    }
    if(savedCommands[BLUE_M]){
      outFadeValue[l][BLUE] = 0;
      outUpdateValue[l][BLUE] = true;
      outLedValue[l][BLUE] = 100;
    }
  }
  if(savedCommands[RED_M_HP]){
    outUpdateValue[HP_LED][RED] = true;
    outLedValue[HP_LED][RED] = 100;
    outFadeValue[HP_LED][RED] = 0;
  }
  if(savedCommands[GREEN_M_HP]){
    outUpdateValue[HP_LED][GREEN] = true;
    outLedValue[HP_LED][GREEN] = 100;
    outFadeValue[HP_LED][GREEN] = 0;
  }
  if(savedCommands[BLUE_M_HP]){
    outUpdateValue[HP_LED][BLUE] = true;
    outLedValue[HP_LED][BLUE] = 100;
    outFadeValue[HP_LED][BLUE] = 0;
  }
  
  // write values to LEDS
  writeOutputValues();
}

bool ReadSerial(byte* outMessage){
    byte tempMessage[2];
    tempMessage[0] = Serial.read();
    while(Serial.available()==0){}
    tempMessage[1] = Serial.read();

    if(tempMessage[0]>100){
      outMessage[0] = tempMessage[0];
      outMessage[1] = tempMessage[1];
      return true;
    }
   return false;   
}

void SaveCommand(byte* inMessage){
  savedCommands[inMessage[0]-101] = inMessage[1];
  switch(inMessage[0]-101){
    case SLIDER_1: sliderAsPeriod[0] = map(inMessage[1],0,100,MAX_PERIOD,MIN_PERIOD); break;
    case SLIDER_2: sliderAsPeriod[1] = map(inMessage[1],0,100,MAX_PERIOD,MIN_PERIOD); break;
    case SLIDER_3: sliderAsPeriod[2] = map(inMessage[1],0,100,MAX_PERIOD,MIN_PERIOD); break;
    case SLIDER_1_HP: sliderAsPeriod[3] = map(inMessage[1],0,100,MAX_PERIOD,MIN_PERIOD); break;   
    default: break;
  }
}

void setOutput(byte led, byte color, byte val, int fade){
 outLedValue[led][color] = val;
 outFadeValue[led][color] = fade;
 outUpdateValue[led][color] = true;
}

void writeOutputValues(){
  for(byte i=0; i<NR_LEDS;i++){
    for(byte k=0; k<NR_COLORS; k++){
      if(outUpdateValue[i][k]){
        if( i < nr_ActiveLeds){
          SoftPWMSetPercent(RGB_LEDS[i][k],(outLedValue[i][k] * savedCommands[SLIDER_MASTER])/100);
          SoftPWMSetFadeTime(RGB_LEDS[i][k],savedCommands[BOOM] ? 0:(outFadeValue[i][k] * savedCommands[FADE]) ,outFadeValue[i][k] * savedCommands[FADE] );
        }else{
          SoftPWMSetPercent(RGB_LEDS[i][k],(outLedValue[i][k] * savedCommands[SLIDER_MASTER_HP])/100 ,outFadeValue[i][k] * savedCommands[FADE_HP]);
          SoftPWMSetFadeTime(RGB_LEDS[i][k],savedCommands[BOOM_HP] ? 0:(outFadeValue[i][k] * savedCommands[FADE_HP]) ,outFadeValue[i][k] * savedCommands[FADE_HP] );
        }
        outUpdateValue[i][k] = false;
      }
    }
  }
}

