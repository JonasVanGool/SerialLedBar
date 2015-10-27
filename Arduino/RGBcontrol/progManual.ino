void _progManual(){
  for(byte l=0; l<nr_ActiveLeds;l++){
    for(byte k=0; k<NR_COLORS; k++){
      outFadeValue[l][k] = 0;
      outUpdateValue[l][k] = true;
    }

   outLedValue[l][0] = savedCommands[SLIDER_1];
   outLedValue[l][1] = savedCommands[SLIDER_2];
   outLedValue[l][2] = savedCommands[SLIDER_3];
  }
}
