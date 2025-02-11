package Hospital.Reservation.Service;

import java.util.NoSuchElementException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;


import Hospital.Reservation.DTO.AppointmentDTO;
import Hospital.Reservation.DTO.AppointmentReadDTO;
import Hospital.Reservation.Entity.Appointment;

@Service
public class AppointmentService {
    @Autowired
    private AppointmentRepository apr;

    public Long BookAppointment(AppointmentDTO apDTO) {
        Appointment appointment = Appointment.builder()
        							.UserId(apDTO.getUserId())
        							.PatientName(apDTO.getPatientName())
        							.AppointmentTime(apDTO.getAppointmentTime()).build();
       this.apr.save(appointment);
       return appointment.getId();
    }
    
	public AppointmentReadDTO AppointmentRead(Long Id) throws NoSuchElementException{
		   Appointment appointment = this.apr.findById(Id).orElseThrow();
		   return AppointmentReadDTO.AppointmentFactory(appointment);
	}
	
	public Appointment saveUserId(String userId) {
        Appointment appointment = new Appointment();
        appointment.setUserId(userId);
        return apr.save(appointment);
	}
}