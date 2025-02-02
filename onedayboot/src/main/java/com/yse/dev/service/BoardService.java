package com.yse.dev.service;

import java.util.ArrayList;
import java.util.List;

import org.springframework.stereotype.Service;

import com.yse.dev.domain.entity.Board;
import com.yse.dev.domain.repository.BoardRepository;
import com.yse.dev.dto.BoardDto;

import jakarta.transaction.Transactional;
//글쓰기 Form에서 내용을 입력한 뒤, '글쓰기' 버튼을 누르면 Post 형식으로 요청이 오고, 
//BoardService의 savePost()를 실행한다.
//service 패키지를 만들고, 그 안에 BoardService 클래스 생성
@Service
public class BoardService {
    private BoardRepository boardRepository;

    public BoardService(BoardRepository boardRepository) {
        this.boardRepository = boardRepository;
    }

    @Transactional
    public Long savePost(BoardDto boardDto) {
        return boardRepository.save(boardDto.toEntity()).getId();
    }

    @Transactional
    public List<BoardDto> getBoardList() {
        List<Board> boardList = boardRepository.findAll();
        List<BoardDto> boardDtoList = new ArrayList<>();

        for(Board board : boardList) {
            BoardDto boardDto = BoardDto.builder()
                    .id(board.getId())
                    .author(board.getAuthor())
                    .title(board.getTitle())
                    .content(board.getContent())
                    .createdDate(board.getCreatedDate())
                    .build();
            boardDtoList.add(boardDto);
        }
        return boardDtoList;
    }
}