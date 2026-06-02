namespace TestManagement.APP.Interfaces
{
    public interface ITestResultReader
    {
        string Read(string path);
    }

    public interface ITesResultReader<T>
    {
        T Read(string path);
    }
}
