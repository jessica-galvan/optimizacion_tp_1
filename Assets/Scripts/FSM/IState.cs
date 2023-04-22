public interface IState<T>
{
    IState<T> GetTransition(T input);

    void AddTransition(T input, IState<T> state);

    void RemoveTransition(T input);

    void RemoveTransition(IState<T> state);

    void Awake();

    void Execute();

    void Sleep();
}
