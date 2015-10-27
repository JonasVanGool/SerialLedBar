byte mainColorHP = 1;
byte mainColorPreHP = 1;
bool isZeroHP = false;

void _progSemiManHP(bool randomColorHP){
  //reset color
  if(programSwitched_HP){
    mainColorHP = 1;
  }
  
  //update color
  if((currentTime - previousTime[2])>sliderAsPeriod[3] && savedCommands[SLIDER_1_HP]!=0 ){
    if(isZeroHP && savedCommands[ZERO_HP]){
      previousTime[2] = currentTime;
      isZeroHP = false;
      outLedValue[HP_LED][RED] = 0;
      outLedValue[HP_LED][GREEN] = 0;
      outLedValue[HP_LED][BLUE] = 0;
      outUpdateValue[HP_LED][RED] = true;
      outUpdateValue[HP_LED][GREEN] = true;
      outUpdateValue[HP_LED][BLUE] = true;
      outFadeValue[HP_LED][RED] = sliderAsPeriod[3];
      outFadeValue[HP_LED][GREEN] = sliderAsPeriod[3];
      outFadeValue[HP_LED][BLUE] = sliderAsPeriod[3];
    }else{
      isZeroHP = true;
      previousTime[2] = currentTime;
      if(randomColorHP){
        while((mainColorHP=random(1,7))==mainColorPreHP){}
          mainColorPreHP = mainColorHP;
      }else{
        if(mainColorHP == 7)
          mainColorHP = 0;
        mainColorHP++;
      }
      outLedValue[HP_LED][RED] = 100*(mainColorHP &  B00000001);
      outLedValue[HP_LED][GREEN] = 100*((mainColorHP & B00000010)>>1);
      outLedValue[HP_LED][BLUE] = 100*((mainColorHP & B00000100)>>2);
      outUpdateValue[HP_LED][RED] = true;
      outUpdateValue[HP_LED][GREEN] = true;
      outUpdateValue[HP_LED][BLUE] = true;
      outFadeValue[HP_LED][RED] = sliderAsPeriod[3];
      outFadeValue[HP_LED][GREEN] = sliderAsPeriod[3];
      outFadeValue[HP_LED][BLUE] = sliderAsPeriod[3];
    } 
  }
}
