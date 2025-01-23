package Patient.DTO;

import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class ExistingHospitalDTO {
	@Id
	private Integer H_Id;
	private String H_Region;
	private String H_Name;
	private String H_Department;
	private Integer Bed_Total;
	private Integer Bed_General;
	private Integer Bed_Psy;
	private Integer Bed_Nursing;
	private Integer Bed_Intensive;
	private Integer Bed_Isolation;
	private Integer Bed_Clean;
	private String H_PhoneNumber;
	private String H_Address;
}
