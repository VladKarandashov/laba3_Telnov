using System.Collections.Generic;

namespace Lab3
{
    class City
    {
        Bus[] buses;
        Queue<Passenger>[] stations;
        int number_of_stops;
        int cap_of_buses;
        public City()
        {
            buses = null;
            stations = null;
            number_of_stops = 0;
            cap_of_buses = 0;
        }
        public City(int number_of_stops, int number_of_buses, int cap_of_buses):this()
        {
            if (number_of_buses < 0) number_of_buses = 0;
            Cap_of_buses = cap_of_buses;
            if (number_of_stops < 0) this.number_of_stops = 0;
            else this.number_of_stops = number_of_stops;
            if (number_of_stops * number_of_buses != 0)
            {
                buses = new Bus[number_of_buses];
                for (int i = 0; i < number_of_buses; i++)
                {
                    buses[i] = new Bus(this.cap_of_buses, new List<Passenger>(), i * this.number_of_stops / number_of_buses, number_of_stops);
                }
                stations = new Queue<Passenger>[this.number_of_stops];
                for (int i = 0; i < this.number_of_stops; i++)
                    stations[i] = new Queue<Passenger>();
            }
        }
        public int Cap_of_buses{
            get { return cap_of_buses; }
            set {
                if (value > 0) cap_of_buses = value;
                else cap_of_buses = 0;
                if (buses != null) foreach (Bus bus in buses) bus.Capacity = cap_of_buses; 
            }
        }
        public Bus[] Buses
        {
            get { return buses; }
        }
        public Queue<Passenger>[] Stations
        {
            get { return stations; }
            set { stations = value; }
        }
        public void Move()
        {
            foreach (Bus bus in buses)
            {
                bus.Cur_station++;
            }
        }
        public void DropOff()
        {
            foreach (Bus bus in buses)
            {
                bus.To_delete = (Passenger pass) => pass.Finish == bus.Cur_station;
                bus.DeletePasseger();
            }
        }
        public void PickUp()
        {
            foreach(Bus bus in buses)
            {
                while (stations[bus.Cur_station].Count != 0 && bus.AddPassenger(stations[bus.Cur_station].Peek())) {
                    stations[bus.Cur_station].Dequeue();
                }
            }
        }
        public void AddPassenger()
        {
            Passenger pass = new Passenger(number_of_stops / 2);
            stations[pass.Start].Enqueue(pass);
        }
        public void CopyPassengers(City other)
        {
            for (int i = 0; i < buses.Length; i++)
            {
                buses[i].Passengers = other.Buses[i].Passengers;
            }
        }
        public void CopyQueues (City other)
        {
            stations = other.Stations;
        }
    }
}
