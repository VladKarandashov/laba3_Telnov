using System;

namespace Lab3
{
    class Passenger
    {
        private int start;
        private int finish;
        public Passenger()
        {
            start = 0;
            finish = 0;
        }
        public Passenger(int start, int finish)
        {
            if (start > 0) this.start = start;
            else this.start = 0;
            if (finish > 0) this.finish = finish;
            else this.finish = 0;
        }
        public Passenger(int max_value)
        {
            Random rnd = new Random();
            start = rnd.Next(0, max_value);
            finish = rnd.Next(0, max_value);
            while (start == finish) finish = rnd.Next(0, max_value);
            if (start > finish)
            {
                start = 2 * max_value - start - 1;
                finish  = 2 * max_value - finish - 1; 
            }
        }
        public int Start {
            get { return start; }
        }
        public int Finish
        {
            get { return finish; }
        }

    }
}
