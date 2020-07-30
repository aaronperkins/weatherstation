using System;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace WeatherStation.Sensors
{
    public class LPS22HBWeatherSensor : IWeatherSensor
    {
        private readonly Unosquare.RaspberryIO.Abstractions.II2CDevice _device;

        public LPS22HBWeatherSensor()
        {
            Pi.Init<BootstrapWiringPi>();
            _device = Pi.I2C.AddDevice(LPS22HB_I2C_ADDRESS);
        }

        public WeatherReading GetReading()
        {
            var result = new WeatherReading();

            LPS22HBInit();

            LPS22HBStartOneShot();

            Pi.Timing.SleepMicroseconds(300);

            float pressureData = 0f;
            float tempData = 0f;

            if ((_device.ReadAddressByte(LPS_STATUS) & 0x01) == 0x01)   // a new pressure data is generated
            {
                byte x0 = _device.ReadAddressByte(LPS_PRESS_OUT_XL);
                byte x1 = _device.ReadAddressByte(LPS_PRESS_OUT_L);
                byte x2 = _device.ReadAddressByte(LPS_PRESS_OUT_H);
                pressureData = (float)((x2 << 16) + (x1 << 8) + x0) / 4096.0f; // hPA Hectopascal Pressure Unit
            }
            if ((_device.ReadAddressByte(LPS_STATUS) & 0x02) == 0x02)   // a new pressure data is generated
            {
                byte x0 = _device.ReadAddressByte(LPS_TEMP_OUT_L);
                byte x1 = _device.ReadAddressByte(LPS_TEMP_OUT_H);
                tempData = (float)((x1 << 8) + x0) / 100.0f; // Celcius
            }

            result.TemperatureC = tempData;
            result.Pressure = pressureData;

            return result;
        }

        private byte LPS22HBInit()
        {
            if (_device.ReadAddressByte(LPS_WHO_AM_I) != LPS_ID) return 0;  //Check device ID 
            LPS22HBReset();                                                 //Wait for reset to complete
            _device.WriteAddressByte(LPS_CTRL_REG1, 0x02);                  //Low-pass filter disabled , output registers not updated until MSB and LSB have been read , Enable Block Data Update , Set Output Data Rate to 0 
            return 1;
        }

        void LPS22HBReset()
        {
            byte Buf;
            Buf = _device.ReadAddressByte(LPS_CTRL_REG2);
            Buf |= 0x04;
            _device.WriteAddressByte(LPS_CTRL_REG2, Buf);
            while (Buf == 1)
            {
                Buf = (byte)(_device.ReadAddressWord(LPS_CTRL_REG2) & 0x04);
            }
        }

        void LPS22HBStartOneShot()
        {
            byte Buf;
            Buf = (byte)(_device.ReadAddressWord(LPS_CTRL_REG2) | 0x01);
            _device.WriteAddressByte(LPS_CTRL_REG2, Buf);
        }

        const byte LPS22HB_I2C_ADDRESS = 0x5C;
        const byte LPS_ID = 0xB1;
        const byte LPS_INT_CFG = 0x0B;      //Interrupt register
        const byte LPS_THS_P_L = 0x0C;      //Pressure threshold registers 
        const byte LPS_THS_P_H = 0x0D;
        const byte LPS_WHO_AM_I = 0x0F;     //Who am I        
        const byte LPS_CTRL_REG1 = 0x10;    //Control registers
        const byte LPS_CTRL_REG2 = 0x11;
        const byte LPS_CTRL_REG3 = 0x12;
        const byte LPS_FIFO_CTRL = 0x14;    //FIFO configuration register 
        const byte LPS_REF_P_XL = 0x15;     //Reference pressure registers
        const byte LPS_REF_P_L = 0x16;
        const byte LPS_REF_P_H = 0x17;
        const byte LPS_RPDS_L = 0x18;       //Pressure offset registers
        const byte LPS_RPDS_H = 0x19;
        const byte LPS_RES_CONF = 0x1A;     //Resolution register
        const byte LPS_INT_SOURCE = 0x25;   //Interrupt register
        const byte LPS_FIFO_STATUS = 0x26;  //FIFO status register
        const byte LPS_STATUS = 0x27;       //Status register
        const byte LPS_PRESS_OUT_XL = 0x28; //Pressure output registers
        const byte LPS_PRESS_OUT_L = 0x29;
        const byte LPS_PRESS_OUT_H = 0x2A;
        const byte LPS_TEMP_OUT_L = 0x2B;   //Temperature output registers
        const byte LPS_TEMP_OUT_H = 0x2C;
        const byte LPS_RES = 0x33;
    }
}
