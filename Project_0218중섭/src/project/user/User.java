package project.user;

import java.security.MessageDigest;
import java.security.SecureRandom;

import project.db.DataBase;



public class User {
	
	private static final int SALT_SIZE = 16;
	private static DataBase db = new DataBase();
	
	public boolean check_UserId(String id) {
		
		if(db.check_Id(id)) {
			System.out.println("중복된 아이디가 있습니다. Id를 다시 설정해주세요");
			return false;
		}
		
		else {
			System.out.println("중복된 아이디가 없습니다. 회원가입을 계속 진행해주세요");
			return true;
		}
	}
	
	
	
	public void set_User(String id, byte[] password) throws Exception{
		String SALT = getSALT();
		db.set_USER(id, Hashing(password, SALT), SALT);
	}
	
	public void get_User(String Id, byte[] password) throws Exception{
		String temp_salt = db.get_SALT(Id);
		String temp_pass = Hashing(password, temp_salt);
		if(db.check(Id, temp_pass)) {
			System.out.println("로그인 성공");
		}
		else {
			System.out.println("로그인 실패");
		}
	}
	
	private String Hashing(byte[] password, String Salt) throws Exception{
		MessageDigest md = MessageDigest.getInstance("SHA-256");
		for(int i = 0; i < 10000; i++) {
			String temp = Byte_to_String(password)+Salt;
			md.update(temp.getBytes());
			password = md.digest();
		}
		return Byte_to_String(password);
	}
	private String getSALT() throws Exception{
		SecureRandom rnd = new SecureRandom();
		byte[] temp = new byte[SALT_SIZE];
		rnd.nextBytes(temp);
		return Byte_to_String(temp);
	}
	
	private String Byte_to_String(byte[] temp) {
		StringBuilder sb = new StringBuilder();
		for(byte a: temp) {
			sb.append(String.format("%02x", a));
		}
		return sb.toString();
	}
	
	public void get_DataBase() {
		System.out.println(db);
	}
}