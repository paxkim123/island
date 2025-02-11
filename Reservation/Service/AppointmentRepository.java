package Hospital.Reservation.Service;

import org.springframework.data.jpa.repository.JpaRepository;

import Hospital.Reservation.Entity.Appointment;

public interface AppointmentRepository extends JpaRepository<Appointment, Long> {
	
}