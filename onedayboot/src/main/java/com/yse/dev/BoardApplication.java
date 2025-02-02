package com.yse.dev;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.data.jpa.repository.config.EnableJpaAuditing;

@EnableJpaAuditing//jpa auditing기능을 사용하기 위한 어노테이션
@SpringBootApplication
public class BoardApplication {
	public static void main(String[] args) {
		SpringApplication.run(BoardApplication.class, args);
	}

}