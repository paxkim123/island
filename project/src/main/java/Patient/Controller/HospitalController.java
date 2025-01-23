package Patient.Controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import Patient.DTO.ExistingHospitalDTO;
import Patient.Service.HospitalService;

@RestController
@RequestMapping("/hospitals")
public class HospitalController {
	@Autowired
	private HospitalService hospitalService;
	
	@GetMapping
	public List<ExistingHospitalDTO> getAllEntities(){
		return hospitalService.getAllEntities();
	}

}
