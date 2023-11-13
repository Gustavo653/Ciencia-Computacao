namespace ProjetoA3;

public class Automaton
{
    private State state = State.Start;

    public bool Accept(string input)
    {
        foreach (var character in input)
        {
            state = Next(character);
            if (state == State.Error)
                return false;
        }

        return true;
    }

    private State Next(char c)
    {
        State nextState = State.Error;
        switch (state)
        {
            case State.Start:
                if (Char.IsLetter(c))
                    nextState = State.Id;
                else if (c == '0')
                    nextState = State.Num;
                else
                    nextState = State.Error;
                break;
            case State.Id:
                nextState = Char.IsLetterOrDigit(c) ? State.Id : State.Error;
                break;
            case State.Num:
                nextState = Char.IsDigit(c) ? State.Num : State.Error;
                break;
            case State.Error:
                break;
        }

        state = nextState;
        return state;
    }
}