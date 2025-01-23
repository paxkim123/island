package Patient.Service;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import Patient.DTO.ExistingHospitalDTO;
import Patient.Entity.Hospital;
import Patient.Entity.HospitalRepository;

@Service
public class HospitalService {
	@Autowired
	private HospitalRepository hospitalRepository;
	public List<ExistingHospitalDTO> getAllEntities(){
		return hospitalRepository.findAll().stream().map(this::convertToDTO).collect(Collectors.toList());
	}
	
	private ExistingHospitalDTO convertToDTO(Hospital h) {
		return new ExistingHospitalDTO(h.getH_Id(), h.getH_Region(), h.getH_Name(), h.getH_Department(), 
										h.getBed_Total(), h.getBed_General(), h.getBed_Psy(), h.getBed_Nursing(), h.getBed_Intensive(), h.getBed_Isolation(), h.getBed_Clean(), h.getH_PhoneNumber(), h.getH_Address());
	}
}