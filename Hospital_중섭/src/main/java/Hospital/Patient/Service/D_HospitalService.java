package Hospital.Patient.Service;


import java.util.List;
//import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import Hospital.Patient.DTO.D_HospitalReadDTO;
import Hospital.Patient.Entity.D_Hospital;
import Hospital.Patient.Entity.D_HospitalRepository;

@Service
public class D_HospitalService {
	@Autowired
	private D_HospitalRepository d_HospitalRepository;
	//@Autowired
	//private PatientInfoRepository patientInfoRepository;
	
	public List<D_Hospital> findAll(){
		return d_HospitalRepository.findAll();
	}
	
	
//	public List<D_HospitalReadDTO> findAll(){
//		return d_HospitalRepository.findAll().stream().map(this::convertToDTO).collect(Collectors.toList());
//	}
	
	public D_HospitalReadDTO save(D_HospitalReadDTO d_HospitalReadDTO) {
		D_Hospital d_Hospital = convertToEntity(d_HospitalReadDTO);
		D_Hospital saveD_Hospital = d_HospitalRepository.save(d_Hospital);
		return convertToDTO(saveD_Hospital);
	}
	
	private D_HospitalReadDTO convertToDTO(D_Hospital d_Hospital) {
		D_HospitalReadDTO d_HospitalReadDTO = new D_HospitalReadDTO();
		d_HospitalReadDTO.setH_Id(d_Hospital.getH_Id());
		d_HospitalReadDTO.setH_Name(d_Hospital.getH_Name());
		d_HospitalReadDTO.setH_Department(d_Hospital.getH_Department());
		d_HospitalReadDTO.setBed_Total(d_Hospital.getBed_Total());
		d_HospitalReadDTO.setBed_General(d_Hospital.getBed_General());
		d_HospitalReadDTO.setBed_Psy(d_Hospital.getBed_Psy());
		d_HospitalReadDTO.setBed_Nursing(d_Hospital.getBed_Nursing());
		d_HospitalReadDTO.setBed_Clean(d_Hospital.getBed_Clean());
		d_HospitalReadDTO.setBed_Intensive(d_Hospital.getBed_Intensive());
		d_HospitalReadDTO.setBed_Isolation(d_Hospital.getBed_Isolation());
		d_HospitalReadDTO.setBed_Clean(d_Hospital.getBed_Clean());
		d_HospitalReadDTO.setH_Phone_Number(d_Hospital.getH_Phone_Number());
		d_HospitalReadDTO.setH_Address(d_Hospital.getH_Address());
		return d_HospitalReadDTO;
	}
	
	private D_Hospital convertToEntity(D_HospitalReadDTO d_HospitalReadDTO) {
		D_Hospital h = new D_Hospital();
		h.setH_Id(h.getH_Id());
		h.setH_Name(h.getH_Name());
		h.setH_Department(h.getH_Department());
		h.setBed_Total(h.getBed_Total());
		h.setBed_General(h.getBed_General());
		h.setBed_Psy(h.getBed_Psy());
		h.setBed_Nursing(h.getBed_Nursing());
		h.setBed_Clean(h.getBed_Clean());
		h.setBed_Intensive(h.getBed_Intensive());
		h.setBed_Isolation(h.getBed_Isolation());
		h.setBed_Clean(h.getBed_Clean());
		h.setH_Phone_Number(h.getH_Phone_Number());
		h.setH_Address(h.getH_Address());
		return h;
	}
	
}


