// Importa namespaces necessários para o funcionamento do serviço, configuração, RabbitMQ e logging
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WorkService.Consumer
{
    // Define uma classe Worker que herda de BackgroundService para ser executada como um serviço em segundo plano
    public class Worker : BackgroundService
    {
        // Declara variáveis privadas para a conexão RabbitMQ, canal e logger
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<Worker> _logger;

        // Construtor da classe Worker que recebe um logger como parâmetro
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            // Cria uma instância de ConnectionFactory com as configurações necessárias para conectar ao RabbitMQ
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost", // Endereço do servidor RabbitMQ
                UserName = "user",          // Nome de usuário para autenticação
                Password = "password",          // Senha para autenticação
                Port = 5672,                // Porta do servidor RabbitMQ
                AutomaticRecoveryEnabled = true, // Habilita a recuperação automática da conexão
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10) // Intervalo de recuperação da rede
            };

            try
            {
                // Tenta criar uma conexão com o RabbitMQ usando a fábrica de conexões configurada
                _connection = connectionFactory.CreateConnection("workservice.consumer");
                // Cria um canal de comunicação com o RabbitMQ a partir da conexão estabelecida
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                // Loga um erro caso ocorra alguma exceção ao tentar conectar ou criar o canal
                _logger.LogError($"Erro ao conectar no RabbitMQ: {ex.Message}");
                throw; // Lança a exceção novamente após logar o erro
            }
        }

        // Método sobreposto que executa o serviço em segundo plano
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Cria um consumidor para a fila do RabbitMQ
            var consumer = new EventingBasicConsumer(_channel);
            // Adiciona um evento para tratar mensagens recebidas
            consumer.Received += Consumer_Received;
            // Inicia o consumo da fila "minha-fila" com o consumidor criado
            _channel.BasicConsume("nota.queue", false, consumer);

            return Task.CompletedTask; // Retorna uma tarefa completada para indicar que a inicialização foi concluída
        }

        // Método assíncrono que trata mensagens recebidas da fila RabbitMQ
        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                // Converte o corpo da mensagem para um array de bytes
                var contentArray = e.Body.ToArray();

                // Converte o array de bytes para uma string usando codificação UTF-8
                var contentString = Encoding.UTF8.GetString(contentArray);

                // Confirma o recebimento da mensagem para o RabbitMQ
                _channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                // Nega a confirmação do recebimento da mensagem em caso de exceção
                _channel.BasicNack(e.DeliveryTag, false, false);

                // Loga um erro caso ocorra uma exceção ao processar a mensagem
                _logger.LogError($"RabbitMQ exception: {ex.Message}");
            }
        }
    }
}