namespace MauiCalc
{
    public partial class MainPage : ContentPage
    {
        public string CurrentInput { get; set; } = String.Empty;

        public string RunningTotal {  get; set; } = String.Empty;

        private string selectedOperator;

        string[] operators = { "+", "-", "x", "/", "=" };

        string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "." };

        bool resetOnNextInput = false;


        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var thisInput = button.Text;

            if (numbers.Contains(thisInput))
            {
                if (resetOnNextInput)
                {
                    CurrentInput = button.Text;
                    resetOnNextInput = false;
                }
                else
                {
                    CurrentInput += button.Text;
                }
                Screen.Text = CurrentInput;
            }
            else if (operators.Contains(thisInput))
            {
                var result = PerformCalculation();
                if (thisInput == "=")
                {
                    CurrentInput = result.ToString();

                    Screen.Text = CurrentInput;

                    RunningTotal = String.Empty;
                    selectedOperator = String.Empty;

                    resetOnNextInput = true;
                }
                else
                {
                    RunningTotal = result.ToString();

                    selectedOperator = thisInput;

                    CurrentInput = String.Empty;

                    Screen.Text = CurrentInput;
                }
            }
        }

        private double PerformCalculation()
        {
            double currentVal;
            double.TryParse(CurrentInput, out currentVal);
 
            double runningVal;
            double.TryParse(RunningTotal, out runningVal);
        
            double result;

            switch (selectedOperator) 
            {
            case "+":
                    result = runningVal + currentVal;
                    break;
                case "-":
                    result = runningVal - currentVal;
                    break;
                case "x":
                    result = runningVal * currentVal;
                    break;
                case "/":
                    result = currentVal == 0 ? 0 : runningVal / currentVal;
                    break;
                default:
                    result = currentVal;
                    break;

            }

            return result;
        }
    }
}
