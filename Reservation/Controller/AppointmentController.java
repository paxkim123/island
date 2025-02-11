package Hospital.Reservation.Controller;

import java.util.NoSuchElementException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;

import org.springframework.web.servlet.ModelAndView;


import Hospital.Reservation.DTO.AppointmentDTO;
import Hospital.Reservation.DTO.AppointmentReadDTO;
import Hospital.Reservation.Service.AppointmentService;
import jakarta.servlet.http.HttpSession;


@Controller
public class AppointmentController {
    @Autowired
    private AppointmentService aps;
    
    
    @GetMapping("/Book")//예약 입력 반는 화면 출력 실행 함수
    public String bookForm() {
        return "Book";
    }
    
	@PostMapping("/Book") 
	public String BookAppointment(AppointmentDTO apDTO, HttpSession session) {
	    String userId = (String) session.getAttribute("UserId");
	    if (userId != null) {
	        apDTO.setUserId(userId); // 세션에서 가져온 UserId를 AppointmentDTO에 설정
	    }
	    Long Id = this.aps.BookAppointment(apDTO);
	    return String.format("redirect:/Home/Result/%s", Id);
	}
	
	@GetMapping("/Book/Result/{Id}") // 제출버튼시 결과창 화면 이동 함수
	public ModelAndView AppointmentResult(@PathVariable("Id") Long Id) throws NoSuchElementException{
		  ModelAndView mav = new ModelAndView();
	      AppointmentReadDTO aprDTO = this.aps.AppointmentRead(Id);
	      mav.addObject("Book",aprDTO);
	      mav.setViewName("Book/Result");
	      return mav;
	   }
	  
}
