package patientDTO;

import java.time.LocalDateTime;

import Hospita.Entity.Patient;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class PatientInfoReadDTO {
   
   private Integer PatientId;
    
   private String Name;
         
   private String Gender;
   private Integer Age;
   private String Phone_Number;
         
   private String Address_1;
   private String Address_2;
         
   private Integer Taking_Pill;
         
   private Integer Nose;
   private Integer Cough;
   private Integer Pain;
   private Integer Diarrhea;
         
   private String High_Risk_Group;
         
   private Integer VAS;
         
   private Integer Agreement;
         
   private LocalDateTime InsertDateTime; 
   
   public PatientInfoReadDTO fromPatientInfo(Patient p) {
      this.PatientId = p.getPatientId();
      this.Name = p.getName();
      this.Gender = p.getGender();
      this.Age = p.getAge();
      this.Phone_Number = p.getPhone_Number();
      this.Address_1 = p.getAddress_1();
      this.Address_2 = p.getAddress_2();
      this.Taking_Pill = p.getTaking_Pill();
      this.Nose = p.getNose();
      this.Cough = p.getCough();
      this.Pain = p.getPain();
      this.Diarrhea = p.getDiarrhea();
      this.High_Risk_Group = p.getHigh_Risk_Group();
      this.VAS = p.getVAS();
      this.Agreement = p.getAgreement();
      this.InsertDateTime = p.getInsertDateTime();
      return this;
   }
   
   public static PatientInfoReadDTO PatientInfoFactory(Patient p) {
      PatientInfoReadDTO pirDTO = new PatientInfoReadDTO();
      pirDTO.fromPatientInfo(p);
      return pirDTO;
   }
}
