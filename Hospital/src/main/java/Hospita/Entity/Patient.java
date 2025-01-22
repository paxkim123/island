package Hospita.Entity;

import java.time.LocalDateTime;

import org.hibernate.annotations.CreationTimestamp;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class Patient {
	 @Id
	   @GeneratedValue(strategy = GenerationType.IDENTITY)
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
	      
	   @CreationTimestamp
	   private LocalDateTime InsertDateTime;   

}