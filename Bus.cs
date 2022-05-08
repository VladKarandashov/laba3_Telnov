using System.Collections.Generic;
namespace Lab3
{
    class Bus
    {
        private int cur_station;
        private int number_of_stops; 
        private int capacity;
        private List<Passenger> passengers;


        public Bus()
        {
            capacity = 0;
            cur_station = 0; 
            passengers = new List<Passenger>();
        }
        public Bus(int capacity) : this()
        {
            Capacity = capacity;
        }
        public Bus(int capacity, List<Passenger> passengers, int cur_station, int number_of_stops) : this()
        {
            Capacity = capacity;
            if (passengers.Count <= capacity) this.passengers = passengers;
            if (number_of_stops > 0) this.number_of_stops = number_of_stops; else this.number_of_stops = 0;
            Cur_station = cur_station;
        }
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value >= 0) capacity = value;
                else capacity = 0;
            }
        }
        public int Cur_station
        {
            get { return cur_station; }
            set
            {
                if (value < 0) cur_station = number_of_stops + value;
                else cur_station = value % number_of_stops;
            }
        }
        public int Number_of_stops
        {
            get { return number_of_stops; }
        }
        public bool AddPassenger(Passenger pass)
        {
            if (passengers.Count < capacity)
            {
                passengers.Add(pass);
                return true;
            }
            return false;
        }
        public List<Passenger> Passengers
        {
            get { return passengers; }
            set { passengers = value; }
        }
        

        public delegate bool ToDelete(Passenger pass);
        private ToDelete to_delete;
        public ToDelete To_delete
        {
            set { to_delete = value; }
        }
        private bool Cond_Del(Passenger pass)
        {
            return to_delete(pass);
        }
        public void DeletePasseger()
        {
            passengers.RemoveAll(Cond_Del);
        }
        public delegate bool ToComp(Passenger pass1, Passenger pass2);
        
        private ToComp to_comp; //true = pass1 < pass2 
        public ToComp To_comp
        {
            set { to_comp = value; }
        }
        public void Sort()
        {
            passengers.Sort(delegate(Passenger pass1, Passenger pass2)
            {
                if (to_comp(pass1, pass2)) return -1;
                else return 1;
            });
        }
    }
}
