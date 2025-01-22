package patientService;



import java.util.NoSuchElementException;

import org.springframework.stereotype.Service;

import Hospita.Entity.Patient;
import Hospita.Entity.PatientInfoRepository;
import lombok.Builder;
import patientDTO.PatientInfoCreateDTO;
import patientDTO.PatientInfoReadDTO;

@Builder
@Service
public class PatientInfoService {
	   private PatientInfoRepository pir;
	   
	   public PatientInfoService(PatientInfoRepository pir) {
	      this.pir = pir;
	   }
	   public Integer PatientInfoInsert(PatientInfoCreateDTO picDTO) {
		      Patient patient_data = Patient.builder()
		                        .Name(picDTO.getName())
		                        .Gender(picDTO.getGender())
		                        .Phone_Number(picDTO.getPhone_Number())
		                        .Address_1(picDTO.getAddress_1())
		                        .Address_2(picDTO.getAddress_2())
		                        .Taking_Pill(picDTO.getTaking_Pill())
		                        .Nose(picDTO.getNose())
		                        .Cough(picDTO.getCough())
		                        .Pain(picDTO.getPain())
		                        .Diarrhea(picDTO.getDiarrhea())
		                        .High_Risk_Group(picDTO.getHigh_Risk_Group())
		                        .VAS(picDTO.getVAS())
		                        .Agreement(picDTO.getAgreement())
		                        .build();
		      this.pir.save(patient_data);
		      return patient_data.getPatientId();
		   }
	   public PatientInfoReadDTO PatientInfoRead(Integer PatientId) throws NoSuchElementException{
		      Patient p = this.pir.findById(PatientId).orElseThrow();
		      return PatientInfoReadDTO.PatientInfoFactory(p);
		   }


}
