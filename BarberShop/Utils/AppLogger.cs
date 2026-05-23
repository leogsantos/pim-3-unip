namespace BarberShop.Utils
{
    /// <summary>
    /// Static logger that writes timestamped entries to logs/app-YYYY-MM-DD.log.
    /// Thread-safe via lock. Call from any controller, service, or helper.
    /// </summary>
    public static class AppLogger
    {
        private static readonly string LogDir =
            Path.Combine(Directory.GetCurrentDirectory(), "logs");

        // Prevents interleaved writes from concurrent requests
        private static readonly object _lock = new();

        public static void Info(string message) => Write("INFO", message);

        public static void Warning(string message) => Write("WARN", message);

        public static void Error(string message, Exception? ex = null)
        {
            string full = ex is null ? message : $"{message} | {ex}";
            Write("ERROR", full);
        }

        private static void Write(string level, string message)
        {
            try
            {
                Directory.CreateDirectory(LogDir);

                string filePath = Path.Combine(LogDir, $"app-{DateTime.Now:yyyy-MM-dd}.log");
                string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level,-5}] {message}";

                lock (_lock)
                {
                    File.AppendAllText(filePath, entry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // If the file write fails we don't crash the app — fall back to stderr
                Console.Error.WriteLine($"[LOGGER ERROR] {ex.Message} | Original: [{level}] {message}");
            }
            finally
            {
                // finally garante que o fluxo de controle seja sempre limpo,
                // mesmo se uma exceção não tratada ocorrer no catch
            }
        }
    }
}
