interface IDataBean<T> {
    T SetValues(params string[] values);

    string[] GetValues();
}