long UPDATE_FREQ[NR_LEDS][NR_COLORS];
int UPDATE_FREQ_WDW[NR_LEDS][NR_COLORS];
int FADE_TIME[NR_LEDS][NR_COLORS];
int MAX_VAL[NR_LEDS][NR_COLORS];
int MIN_VAL[NR_LEDS][NR_COLORS];
long PRE_TIME[NR_LEDS][NR_COLORS];

void _progFire(){
  
  for(byte l=0; l<nr_ActiveLeds;l++){
   MAX_VAL[l][0] = savedCommands[SLIDER_1];
   MAX_VAL[l][1] = savedCommands[SLIDER_2];
   MAX_VAL[l][2] = savedCommands[SLIDER_3];
   
   MIN_VAL[l][0] = max(0,savedCommands[SLIDER_1]-50);
   MIN_VAL[l][1] = max(0,savedCommands[SLIDER_2]-50);
   MIN_VAL[l][2] = max(0,savedCommands[SLIDER_3]-50);
   
   for(byte k=0; k<NR_COLORS; k++){
    outFadeValue[l][k] = FADE_TIME[l][k];
    if((currentTime-PRE_TIME[l][k])>random(UPDATE_FREQ[l][k]-UPDATE_FREQ_WDW[l][k],UPDATE_FREQ[l][k]+UPDATE_FREQ_WDW[l][k])){
      outLedValue[l][k] = random(MIN_VAL[l][k],MAX_VAL[l][k]);
      outUpdateValue[l][k] = true;
      PRE_TIME[l][k] = currentTime;
    }
   }
  }
}

void initFire(){
 for(byte j=0; j<nr_ActiveLeds; j++){
  for(byte i=0; i<NR_COLORS;i++){
    UPDATE_FREQ[j][i] = 100;
    UPDATE_FREQ_WDW[j][i] = 20;
    FADE_TIME[j][i] = 100;
  }
 }
}
