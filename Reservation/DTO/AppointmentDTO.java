package Hospital.Reservation.DTO;

import java.time.LocalDateTime;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class AppointmentDTO {
	private String UserId;
    private String patientName;
    private LocalDateTime appointmentTime;
    // Getters and Setters
}