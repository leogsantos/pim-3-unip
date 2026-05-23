namespace BarberShop.Utils
{
    /// <summary>
    /// Logger estático que grava entradas com timestamp em logs/app-AAAA-MM-DD.log.
    /// Thread-safe via lock. Pode ser chamado de qualquer controller, serviço ou helper.
    /// </summary>
    public static class AppLogger
    {
        private static readonly string LogDir =
            Path.Combine(Directory.GetCurrentDirectory(), "logs");

        // Evita gravações intercaladas em requisições concorrentes
        private static readonly object _lock = new();

        public static void Info(string mensagem) => Write("INFO", mensagem);

        public static void Warning(string mensagem) => Write("WARN", mensagem);

        public static void Error(string mensagem, Exception? ex = null)
        {
            string completo = ex is null ? mensagem : $"{mensagem} | {ex}";
            Write("ERROR", completo);
        }

        private static void Write(string nivel, string mensagem)
        {
            try
            {
                Directory.CreateDirectory(LogDir);

                string caminhoArquivo = Path.Combine(LogDir, $"app-{DateTime.Now:yyyy-MM-dd}.log");
                string entrada = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nivel,-5}] {mensagem}";

                lock (_lock)
                {
                    File.AppendAllText(caminhoArquivo, entrada + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // Se a gravação falhar, não derruba a aplicação — redireciona para stderr
                Console.Error.WriteLine($"[ERRO NO LOGGER] {ex.Message} | Original: [{nivel}] {mensagem}");
            }
            finally
            {
                // finally garante que o fluxo de controle seja sempre limpo,
                // mesmo se uma exceção não tratada ocorrer no catch
            }
        }
    }
}
