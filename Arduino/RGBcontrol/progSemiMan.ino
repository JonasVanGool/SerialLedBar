byte mainColor = 1;
bool isZero = false;
byte mainColorPre = 1;
byte subColor = 7;
byte lengthSubColor = 1;
byte subColorPos = 0;
byte MAX_LENGTH_SUBCOLOR = 5;
bool updateValues = true;

void _progSemiMan(bool randomColor){
  // reset color
  if(programSwitched){
    mainColor = 1;
  }
  
  //update color
  if((currentTime - previousTime[0])>sliderAsPeriod[0] && savedCommands[SLIDER_1]!=0 ){
    if(isZero && savedCommands[ZERO]){
      previousTime[0] = currentTime;
      isZero = false;
      updateValues = true;
    }else{
      isZero = true;
      previousTime[0] = currentTime;
      if(randomColor){
        while((mainColor=random(1,7))==mainColorPre){}
          mainColorPre = mainColor;
      }else{
        if(mainColor == 7)
          mainColor = 0;
        mainColor++;
      }
      //calculate counter color
      subColor = (mainColor + 4)>7 ?((mainColor + 4)-7):(mainColor + 4);
      updateValues = true;
    }
  }
  
  //calculate length sub color
  lengthSubColor = map(savedCommands[SLIDER_3],0,100,0,MAX_LENGTH_SUBCOLOR);
  
  //update position
  if((currentTime - previousTime[1])> sliderAsPeriod[1] && savedCommands[SLIDER_2]!=0 ){
    previousTime[1] = currentTime;
    subColorPos++;
    if(subColorPos == MAX_LENGTH_SUBCOLOR){
      subColorPos = 0;
    }
    updateValues = true;
  }
  
  if(updateValues){
    updateValues = false;
    for(byte l=0; l<nr_ActiveLeds;l++){
      for(byte k=0; k<NR_COLORS; k++){
        outUpdateValue[l][k] = true;
      }
      if(l>=subColorPos && l<(subColorPos+lengthSubColor)){
       outLedValue[l][0] = !isZero ? 0 : (100*(subColor &  B00000001));
       outLedValue[l][1] = !isZero ? 0 : (100*((subColor & B00000010)>>1));
       outLedValue[l][2] = !isZero ? 0 : (100*((subColor & B00000100)>>2));
       outFadeValue[l][0] = sliderAsPeriod[1];
       outFadeValue[l][1] = sliderAsPeriod[1];
       outFadeValue[l][2] = sliderAsPeriod[1];
      }else{
       outLedValue[l][0] = !isZero ? 0 : (100*(mainColor &  B00000001));
       outLedValue[l][1] = !isZero ? 0 : (100*((mainColor & B00000010)>>1));
       outLedValue[l][2] = !isZero ? 0 : (100*((mainColor & B00000100)>>2));
       outFadeValue[l][0] = sliderAsPeriod[0];
       outFadeValue[l][1] = sliderAsPeriod[0];
       outFadeValue[l][2] = sliderAsPeriod[0];
      }
    }
  }
}
