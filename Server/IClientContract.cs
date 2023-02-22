namespace Server;

public interface IClientContract { 
    public Task<Object> UnhandledClientResultMethod();
    public Task<Object> UnhandledClientResultMethodWithArg(Object arg);
}