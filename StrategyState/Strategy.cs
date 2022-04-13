namespace StrategyState;

public interface IStrategy
{
    double Operation(double firstOperand, double secondOperand);
}

public class Addition : IStrategy
{
    public double Operation(double firstOperand, double secondOperand)
        => firstOperand + secondOperand;
}

public class Subtract : IStrategy
{
    public double Operation(double firstOperand, double secondOperand)
        => firstOperand - secondOperand;
}

public class Multiply : IStrategy
{
    public double Operation(double firstOperand, double secondOperand)
        => firstOperand * secondOperand;
}

public class Division : IStrategy
{
    public double Operation(double firstOperand, double secondOperand)
        => firstOperand / secondOperand;
}


public class Calculator
{
    private IStrategy? _strategy;
    public IStrategy Strategy { set => _strategy = value; }


    public double? Calculate(double firstOperand, double secondOperand)
        => _strategy?.Operation(firstOperand, secondOperand);
}
