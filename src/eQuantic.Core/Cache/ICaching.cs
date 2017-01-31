namespace eQuantic.Core.Cache
{
    public interface ICaching
    {
        /// <summary>
        /// Insere valor em cache usando os parâmetros apropriados
        /// </summary>
        /// <typeparam name="T">Tipo Objeto Cache</typeparam>
        /// <param name="object"></param>
        /// <param name="key">Nome item</param>
        void Add<T>(T @object, string key);

        /// <summary>
        /// Insere valor em cache usando os parâmetros apropriados
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="object">Object to cache</param>
        /// <param name="key">Item key</param>
        /// <param name="timeout">Total in minutes</param>
        void Add<T>(T @object, string key, double timeout);

        /// <summary>
        /// Remove item do cache
        /// </summary>
        /// <param name="key">Nome item</param>
        void Remove(string key);

        /// <summary>
        /// Verifica existencia do item no chace
        /// </summary>
        /// <param name="key">Nome item</param>
        /// <returns></returns>
        bool IsNull(string key);

        /// <summary>
        /// Recuperar item do Cache
        /// </summary>
        /// <typeparam name="T">Tipo Objeto</typeparam>
        /// <param name="key">Nome item</param>
        /// <returns>Objeto tipado</returns>
        T Get<T>(string key);

        string[] AllKeys();
    }
}
