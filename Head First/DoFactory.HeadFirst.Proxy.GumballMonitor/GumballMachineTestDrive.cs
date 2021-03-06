using System;
using System.Text;

namespace DoFactory.HeadFirst.Proxy.GumballMonitor
{
    class GumballMachineTestDrive
    {
        static void Main(string[] args)
        {
            string location = "Seattle";
            int count = 122;

            var gumballMachine = new GumballMachine(location, count);
            var monitor = new GumballMonitor(gumballMachine);

            Console.WriteLine(gumballMachine);

            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();
            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();

            Console.WriteLine(gumballMachine);

            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();
            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();

            Console.WriteLine(gumballMachine);

            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();
            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();

            Console.WriteLine(gumballMachine);

            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();
            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();

            Console.WriteLine(gumballMachine);

            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();
            gumballMachine.InsertQuarter();
            gumballMachine.TurnCrank();

            Console.WriteLine(gumballMachine);

            monitor.Report();

            // Wait for user
            Console.ReadKey();
        }
    }

    #region Gumball Monitor

    public class GumballMonitor
    {
        private GumballMachine _machine;

        public GumballMonitor(GumballMachine machine)
        {
            this._machine = machine;
        }

        public void Report()
        {
            Console.WriteLine("Gumball Machine: " + _machine.Location);
            Console.WriteLine("Current inventory: " + _machine.Count + " gumballs");
            Console.WriteLine("Current state: " + _machine.State);
        }
    }

    #endregion

    #region Gumball Machine

    public class GumballMachine
    {
        private IState _soldOutState;
        private IState _noQuarterState;
        private IState _hasQuarterState;
        private IState _soldState;
        private IState _winnerState;

        private IState _state;

        public int Count { get; private set; }
        public string Location { get; private set; }

        public GumballMachine(string location, int count)
        {
            _soldOutState = new SoldOutState(this);
            _noQuarterState = new NoQuarterState(this);
            _hasQuarterState = new HasQuarterState(this);
            _soldState = new SoldState(this);
            _winnerState = new WinnerState(this);

            Location = location;
            Count = count;

            if (Count > 0)
                _state = _noQuarterState;
            else
                _state = _soldOutState;
        }

        public void InsertQuarter()
        {
            _state.InsertQuarter();
        }

        public void EjectQuarter()
        {
            _state.EjectQuarter();
        }

        public void TurnCrank()
        {
            _state.TurnCrank();
            _state.Dispense();
        }

        public IState State
        {
            set { _state = value; }
            get { return _state; }
        }

        public void ReleaseBall()
        {
            if (Count > 0)
            {
                Console.WriteLine("A gumball comes rolling out the slot...");
                Count--;
            }
        }

        void Refill(int count)
        {
            Count = count;
            _state = _noQuarterState;
        }


        public IState GetSoldOutState()
        {
            return _soldOutState;
        }

        public IState GetNoQuarterState()
        {
            return _noQuarterState;
        }

        public IState GetHasQuarterState()
        {
            return _hasQuarterState;
        }

        public IState GetSoldState()
        {
            return _soldState;
        }

        public IState GetWinnerState()
        {
            return _winnerState;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("\nMighty Gumball, Inc.");
            result.Append("\n.NET-enabled Standing Gumball Model #2004");
            result.Append("\nInventory: " + Count + " gumball");
            if (Count != 1)
            {
                result.Append("s");
            }
            result.Append("\n");
            result.Append("Machine is " + _state + "\n");
            return result.ToString();
        }
    }

    #endregion

    #region State

    public interface IState
    {
        void InsertQuarter();
        void EjectQuarter();
        void TurnCrank();
        void Dispense();
    }

    public class SoldState : IState
    {
        private GumballMachine _machine;

        public SoldState(GumballMachine machine)
        {
            this._machine = machine;
        }

        public void InsertQuarter()
        {
            Console.WriteLine("Please wait, we're already giving you a gumball");
        }

        public void EjectQuarter()
        {
            Console.WriteLine("Sorry, you already turned the crank");
        }

        public void TurnCrank()
        {
            Console.WriteLine("Turning twice doesn't get you another gumball!");
        }

        public void Dispense()
        {
            _machine.ReleaseBall();
            if (_machine.Count > 0)
            {
                _machine.State = _machine.GetNoQuarterState();
            }
            else
            {
                Console.WriteLine("Oops, out of gumballs!");
                _machine.State = _machine.GetSoldOutState();
            }
        }

        public override string ToString()
        {
            return "dispensing a gumball";
        }
    }

    public class SoldOutState : IState
    {
        private GumballMachine _machine;

        public SoldOutState(GumballMachine machine)
        {
            this._machine = machine;
        }

        public void InsertQuarter()
        {
            Console.WriteLine("You can't insert a quarter, the machine is sold out");
        }

        public void EjectQuarter()
        {
            Console.WriteLine("You can't eject, you haven't inserted a quarter yet");
        }

        public void TurnCrank()
        {
            Console.WriteLine("You turned, but there are no gumballs");
        }

        public void Dispense()
        {
            Console.WriteLine("No gumball dispensed");
        }

        public override string ToString()
        {
            return "sold out";
        }
    }

    public class NoQuarterState : IState
    {
        private GumballMachine _machine;

        public NoQuarterState(GumballMachine machine)
        {
            this._machine = machine;
        }

        public void InsertQuarter()
        {
            Console.WriteLine("You inserted a quarter");
            _machine.State = _machine.GetHasQuarterState();
        }

        public void EjectQuarter()
        {
            Console.WriteLine("You haven't inserted a quarter");
        }

        public void TurnCrank()
        {
            Console.WriteLine("You turned, but there's no quarter");
        }

        public void Dispense()
        {
            Console.WriteLine("You need to pay first");
        }

        public override string ToString()
        {
            return "waiting for quarter";
        }
    }

    public class HasQuarterState : IState
    {
        private GumballMachine _machine;

        public HasQuarterState(GumballMachine machine)
        {
            this._machine = machine;
        }

        public void InsertQuarter()
        {
            Console.WriteLine("You can't insert another quarter");
        }

        public void EjectQuarter()
        {
            Console.WriteLine("Quarter returned");
            _machine.State = _machine.GetNoQuarterState();
        }

        public void TurnCrank()
        {
            Console.WriteLine("You turned...");
            Random random = new Random();

            int winner = random.Next(11);
            if ((winner == 0) && (_machine.Count > 1))
            {
                _machine.State = _machine.GetWinnerState();
            }
            else
            {
                _machine.State = _machine.GetSoldState();
            }
        }

        public void Dispense()
        {
            Console.WriteLine("No gumball dispensed");
        }

        public override string ToString()
        {
            return "waiting for turn of crank";
        }
    }

    public class WinnerState : IState
    {
        private GumballMachine _machine;

        public WinnerState(GumballMachine machine)
        {
            this._machine = machine;
        }

        public void InsertQuarter()
        {
            Console.WriteLine("Please wait, we're already giving you a Gumball");
        }

        public void EjectQuarter()
        {
            Console.WriteLine("Please wait, we're already giving you a Gumball");
        }

        public void TurnCrank()
        {
            Console.WriteLine("Turning again doesn't get you another gumball!");
        }

        public void Dispense()
        {
            Console.WriteLine("YOU'RE A WINNER! You get two gumballs for your quarter");
            _machine.ReleaseBall();
            if (_machine.Count == 0)
            {
                _machine.State = _machine.GetSoldOutState();
            }
            else
            {
                _machine.ReleaseBall();
                if (_machine.Count > 0)
                {
                    _machine.State = _machine.GetNoQuarterState();
                }
                else
                {
                    Console.WriteLine("Oops, out of gumballs!");
                    _machine.State = _machine.GetSoldOutState();
                }
            }
        }

        public override string ToString()
        {
            return "despensing two gumballs for your quarter, because YOU'RE A WINNER!";
        }
    }
    #endregion
}
