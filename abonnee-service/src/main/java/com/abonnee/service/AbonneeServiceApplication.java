package com.abonnee.service;

import org.apache.camel.main.Main;

public class AbonneeServiceApplication {

	public static void main(String[] args) throws Exception {
		Main main = new Main();
		main.configure().addRoutesBuilder(new AbonneeRouteBuilder());
		main.run(args);
	}

}
