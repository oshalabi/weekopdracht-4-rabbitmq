package com.abonnee.service;

import org.apache.camel.builder.RouteBuilder;
public class AbonneeRouteBuilder extends RouteBuilder {

    @Override
    public void configure() throws Exception {

        log.info("AbonneeService is now listening for messages on topic: {{rabbitmq.exchange}} with routing key: {{rabbitmq.routingKey}}");


        from("rabbitmq://{{rabbitmq.host}}:{{rabbitmq.port}}/{{rabbitmq.exchange}}"
        + "?username={{rabbitmq.username}}"
        + "&password={{rabbitmq.password}}"
        + "&exchangeType={{rabbitmq.exchangeType}}"
        + "&routingKey={{rabbitmq.routingKey}}"
        + "&autoAck={{rabbitmq.autoAck}}"
        + "&durable={{rabbitmq.durable}}"
        + "&autoDelete={{rabbitmq.autoDelete}}")
        .log("Received message from RabbitMQ. Exchange: {{rabbitmq.exchange}}, Routing Key: {{rabbitmq.routingKey}}")
        .log("Message body: User ${body}")
        .process(exchange -> {
            String messageBody = exchange.getIn().getBody(String.class);
            System.out.println("Processing message for Abonnee: " + messageBody);
        });
    }
}
