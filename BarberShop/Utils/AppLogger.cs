namespace BarberShop.Utils
{
    /// <summary>
    /// Logger estático que grava entradas com timestamp em logs/app-AAAA-MM-DD.log.
    /// Thread-safe via lock. Pode ser chamado de qualquer controller, serviço ou helper.
    /// </summary>
    public static class AppLogger
    {
        private static string LogDir = "";
        private static bool _initialized = false;
        private static readonly object _lock = new();

        public static void Init(string contentRootPath)
        {
            // Sobe na árvore de pastas até encontrar o .csproj (raiz do projeto),
            // porque ao debugar no VS o ContentRootPath aponta para bin/Debug/net*/
            var dir = contentRootPath;
            while (!string.IsNullOrEmpty(dir) && Directory.GetFiles(dir, "*.csproj").Length == 0)
                dir = Path.GetDirectoryName(dir);

            var projetoRaiz = dir ?? contentRootPath;
            LogDir = Path.Combine(projetoRaiz, "logs");

            try
            {
                // Cria a pasta se não existir
                Directory.CreateDirectory(LogDir);
                _initialized = true;

                // Log de inicialização
                Write("INIT", $"=== APLICAÇÃO INICIADA ===");
                Write("INIT", $"Pasta de logs: {LogDir}");
                Write("INIT", $"ContentRootPath: {contentRootPath}");

                Console.WriteLine($"AppLogger inicializado: {LogDir}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"ERRO ao inicializar AppLogger: {ex.Message}");
                Console.Error.WriteLine($"  Tentou criar em: {LogDir}");
            }
        }

        public static void Info(string mensagem) => Write("INFO", mensagem);
        public static void Warning(string mensagem) => Write("WARN", mensagem);
        public static void Debug(string mensagem) => Write("DEBUG", mensagem);

        public static void Error(string mensagem, Exception? ex = null)
        {
            string completo = ex is null
                ? mensagem
                : $"{mensagem}\n  Exceção: {ex.GetType().Name}\n  Mensagem: {ex.Message}\n  StackTrace: {ex.StackTrace}";
            Write("ERROR", completo);
        }

        private static void Write(string nivel, string mensagem)
        {
            // Se não foi inicializado, tenta usar fallback
            if (!_initialized || string.IsNullOrEmpty(LogDir))
            {
                LogDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                Directory.CreateDirectory(LogDir);
            }

            string caminhoArquivo = "";

            try
            {
                caminhoArquivo = Path.Combine(LogDir, $"app-{DateTime.Now:yyyy-MM-dd}.log");
                string entrada = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nivel,-5}] {mensagem}";

                lock (_lock)
                {
                    File.AppendAllText(caminhoArquivo, entrada + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // Se falhar, mostra erro detalhado no console (janela Output do VS)
                Console.Error.WriteLine($"╔════════════════════════════════════════════════════════════════");
                Console.Error.WriteLine($"║ ERRO AO GRAVAR LOG");
                Console.Error.WriteLine($"╠════════════════════════════════════════════════════════════════");
                Console.Error.WriteLine($"║ Pasta LogDir: {LogDir}");
                Console.Error.WriteLine($"║ Arquivo tentado: {caminhoArquivo}");
                Console.Error.WriteLine($"║ Exceção: {ex.GetType().Name}");
                Console.Error.WriteLine($"║ Mensagem: {ex.Message}");
                Console.Error.WriteLine($"║ Log original: [{nivel}] {mensagem}");
                Console.Error.WriteLine($"╚════════════════════════════════════════════════════════════════");
            }
            finally
            {
                // finally garante que o fluxo de controle seja sempre limpo
            }
        }
    }
}
