package com.abonnee.service;

import org.apache.camel.builder.RouteBuilder;
public class AbonneeRouteBuilder extends RouteBuilder {

    @Override
    public void configure() throws Exception {
        from("rabbitmq:notification?portNumber=5672&exchangeType=topic&routingKey=notification_abonnee&autoAck=false&durable=false&autoDelete=false")
        .log("Received message from RabbitMQ: ${body}")
        .process(exchange -> {
            String messageBody = exchange.getIn().getBody(String.class);
            // Process the message for Abonnee
            System.out.println("Processing message for Abonnee: " + messageBody);
        });
    }
}
