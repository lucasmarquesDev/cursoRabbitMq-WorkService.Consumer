Este projeto é um exemplo de consumidor RabbitMQ implementado em C# usando a biblioteca RabbitMQ.Client. Destina-se a alunos que estão aprendendo sobre RabbitMQ e desejam construir aplicativos que interagem com filas de mensagens.

### Funcionalidades principais:

- **Conexão e Configuração**: O projeto demonstra como configurar e estabelecer uma conexão com um servidor RabbitMQ, incluindo a definição de credenciais de autenticação, host e porta.
  
- **Consumo de Mensagens**: Utiliza a classe `EventingBasicConsumer` para consumir mensagens de uma fila RabbitMQ. Quando uma mensagem é recebida, ela é processada pelo método `Consumer_Received`.
  
- **Tratamento de Exceções**: Implementa tratamento de exceções para lidar com possíveis erros durante o processamento de mensagens, garantindo a integridade do fluxo de mensagens.

### Como usar:

1. Configure as credenciais de acesso ao RabbitMQ no construtor da classe `Worker`.
2. Substitua o nome da fila na chamada `BasicConsume` dentro do método `ExecuteAsync` pelo nome da fila que deseja consumir.
3. Implemente a lógica de processamento da mensagem dentro do método `Consumer_Received`.
4. Execute o projeto para iniciar o consumo de mensagens da fila especificada.

Este projeto serve como uma base sólida para a construção de aplicativos consumidores RabbitMQ em C# e pode ser estendido e adaptado de acordo com os requisitos específicos do seu projeto.
