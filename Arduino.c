#include <Servo.h>

Servo myservo1;
Servo myservo2; 
// create servo object to control a servo
// twelve servo objects can be created on most boards

int pos = 0;    // variable to store the servo position
int firstSensor = 0;
int inByte1 = 0;
int inByte2 = 0;

void setup() {
  myservo2.attach(9);  // attaches the servo on pin 9 to the servo object
  myservo1.attach(10);  // attaches the servo on pin 9 to the servo object
 pinMode(13, OUTPUT);
 Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
 
}
 void loop() {
  if (Serial.available() > 1) {
   inByte1 = Serial.read();
   inByte2 = Serial.read();
 // digitalWrite(13, HIGH);   // turn the LED on (HIGH is the voltage level)
  //delay(1000);
  //digitalWrite(13, LOW);    // turn the LED off by making the voltage LOW
  //delay(1000); 
    myservo1.write(inByte1);                           
    myservo2.write(inByte2);                                                     
  }
}
