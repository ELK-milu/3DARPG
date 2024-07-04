public interface ISingleTon<T>
{
    // 定义一个标识确保线程同步
    public object locker { get; set; }
    public T SingleTon { get; }
}
