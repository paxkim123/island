package Hospital.Login.DTO;

import jakarta.persistence.Id;
import lombok.Getter;
import lombok.NonNull;

@Getter
public class UserLoginDTO {
	@Id
	@NonNull
	private String UserId;
	@NonNull
	private String UserPw;
}
