package controller;

import java.util.NoSuchElementException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.servlet.ModelAndView;

import patientDTO.PatientInfoCreateDTO;
import patientDTO.PatientInfoReadDTO;
import patientService.PatientInfoService;

public class PatientInfoController {
	 @Autowired
	   private PatientInfoService pis;
	   
	   @GetMapping("/Patient/Info/Input") // 문진표 입력 받는 화면 출력 실행 함수
	   public String InfoInputPrint() {
	      return "/Patient/Info/Input";
	   }
	   
	   @PostMapping("/Patient/Info/Insert") // 문진표 입력 받는 화면에서 제출 버튼 누르면 이동하는 함수
	   public String InfoInputInsert(PatientInfoCreateDTO picDTO) {
	      Integer PatientId = this.pis.PatientInfoInsert(picDTO);
	      return String.format("redirect:/Patient/Info/Result/%s", PatientId);
	   }
	   @GetMapping("/Patient/Info/Result/{PatientId}") // 제출버튼시 결과창 화면 이동 함수
	   public ModelAndView Result(@PathVariable("PatientId") Integer PatientId) throws NoSuchElementException{
	      ModelAndView mav = new ModelAndView();
	      PatientInfoReadDTO pirDTO = this.pis.PatientInfoRead(PatientId);
	      mav.addObject("PatientData",pirDTO);
	      mav.setViewName("Patient/Info/Result");
	      return mav;
	   }


}
