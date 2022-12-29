using System;
using DddInPractice.Logic.Atms;
using DddInPractice.Logic.SharedKernel;
using DddInPractice.UI.Common;

namespace DddInPractice.UI.Atms
{
    public class AtmViewModel : ViewModel
    {
        private readonly Atm _atm;
        private readonly AtmRepository _atmRepository;
        private readonly PaymentGateway _paymentGateway;

        private string _message;
        public string Message
        {
            get { return _message; }
            private set
            {
                _message = value;
                Notify();
            }
        }

        public override string Caption => "ATM";
        public Money MoneyInside => _atm.MoneyInside;
        public string MoneyCharged => _atm.MoneyCharged.ToString("C2");
        public Command<decimal> TakeMoneyCommand { get; private set; }

        public AtmViewModel(Atm atm, AtmRepository atmRepository, PaymentGateway paymentGateway)
        {
            _atm = atm;
            _atmRepository = atmRepository;
            _paymentGateway = paymentGateway;

            TakeMoneyCommand = new Command<decimal>(x => x > 0, TakeMoney);
        }

        private void TakeMoney(decimal amount)
        {
            var error = _atm.CanTakeMoney(amount);
            
            if (error != string.Empty)
            {
                NotifyClient(error);
                return;
            }

            decimal amountWithCommission = _atm.CalculateAmountWithCommission(amount);
            _paymentGateway.ChargePayment(amountWithCommission);
            _atm.TakeMoney(amount);
            _atmRepository.Save(_atm);
            
            NotifyClient("You have taken " + amount.ToString("C2"));
        }

        private void NotifyClient(string message)
        {
            Message = message;
            Notify(nameof(MoneyInside));
            Notify(nameof(MoneyCharged));
        }
    }
}
