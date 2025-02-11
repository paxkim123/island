package Hospital.Reservation.DTO;

import java.time.LocalDateTime;

import Hospital.Reservation.Entity.Appointment;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@NoArgsConstructor
public class AppointmentReadDTO {
    private Long Id;
    private String UserId;
    private String PatientName;
    private LocalDateTime AppointmentTime;
    
    public AppointmentReadDTO FromAppointment(Appointment a) {
    	this.Id = a.getId();
    	this.UserId = a.getUserId();
    	this.PatientName = a.getPatientName();
    	this.AppointmentTime = a.getAppointmentTime();
    	return this;
    }
    
    public static AppointmentReadDTO AppointmentFactory(Appointment a) {
    	AppointmentReadDTO arDTO = new AppointmentReadDTO();
    	arDTO.FromAppointment(a);
    	return arDTO;
    }
}
