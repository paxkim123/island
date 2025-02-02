package com.yse.dev.domain.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.yse.dev.domain.entity.Board;

public interface BoardRepository extends JpaRepository<Board, Long> {

}
