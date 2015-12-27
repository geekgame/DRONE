namespace Drone.Core.LowLevel.GPIO
{
    internal class servo
    {
        public static int gpioToID(int gpio)
        {
            switch (gpio)
            {
                case 4:
                    return 0;
                case 17:
                    return 1;
                case 18:
                    return 2;
                case 21:
                    return 3;
                case 22:
                    return 4;
                case 23:
                    return 5;
                case 24:
                    return 6;
                case 25:
                    return 7;
                default:
                    return 8;
            }
        }

        public static void initServoBlaster()
        {
        }
    }
}