void _progHPRed(){
    outLedValue[HP_LED][RED] = 100;
    outLedValue[HP_LED][GREEN] = 0;
    outLedValue[HP_LED][BLUE] = 0;
    outUpdateValue[HP_LED][RED] = true;
    outUpdateValue[HP_LED][GREEN] = true;
    outUpdateValue[HP_LED][BLUE] = true;
}

void _progHPGreen(){
    outLedValue[HP_LED][RED] = 0;
    outLedValue[HP_LED][GREEN] = 100;
    outLedValue[HP_LED][BLUE] = 0;
    outUpdateValue[HP_LED][RED] = true;
    outUpdateValue[HP_LED][GREEN] = true;
    outUpdateValue[HP_LED][BLUE] = true;
}

void _progHPBlue(){
    outLedValue[HP_LED][RED] = 0;
    outLedValue[HP_LED][GREEN] = 0;
    outLedValue[HP_LED][BLUE] = 100;
    outUpdateValue[HP_LED][RED] = true;
    outUpdateValue[HP_LED][GREEN] = true;
    outUpdateValue[HP_LED][BLUE] = true;
}
