namespace ProjetoA3;

public class Automaton
{
    private State state = State.START;

    public State Next(char c)
    {
        State nextState = State.ERROR;
        switch (state)
        {
            case State.START:
                if (Char.IsLetter(c))
                    nextState = State.ID;
                else if (c == '0')
                    nextState = State.NUM;
                else
                    nextState = State.ERROR;
                break;
            case State.ID:
                nextState = Char.IsLetterOrDigit(c) ? State.ID : State.ERROR;
                break;
            case State.NUM:
                nextState = Char.IsDigit(c) ? State.NUM : State.ERROR;
                break;
            case State.ERROR:
                break;
        }

        state = nextState;
        return state;
    }

    public bool Accept()
    {
        return state == State.START;
    }
}