-- ======================================
-- CREATE DATABASE
-- ======================================
DROP DATABASE IF EXISTS graphanetracedb;
CREATE DATABASE graphanetracedb;
USE graphanetracedb;

-- ======================================
-- TABLE: roles
-- ======================================
CREATE TABLE `roles` (
  `role_id` int NOT NULL AUTO_INCREMENT,
  `role_name` varchar(50) NOT NULL,
  PRIMARY KEY (`role_id`),
  UNIQUE KEY `role_name` (`role_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO roles (role_id, role_name) VALUES
(1, 'Admin'),
(2, 'Clinician'),
(3, 'Patient');

-- ======================================
-- TABLE: users
-- ======================================
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `full_name` varchar(150) NOT NOT NULL,
  `email` varchar(150) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `role_id` int NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `email` (`email`),
  KEY `role_id` (`role_id`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `roles` (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO users (user_id, full_name, email, password_hash, role_id, created_at) VALUES
(1, 'System Administrator', 'admin@graphenetrace.local', 'admin123', 1, '2025-11-24 19:34:18'),
(2, 'Dr. Emily Carter', 'clinician1@graphenetrace.local', 'clinician123', 2, '2025-11-24 19:34:18'),
(3, 'Dr. Rajesh Kumar', 'clinician2@graphenetrace.local', 'clinician123', 2, '2025-11-24 19:34:18'),
(4, 'Dr. Sarah Lee', 'clinician3@graphenetrace.local', 'clinician123', 2, '2025-11-24 19:34:18'),
(5, 'John Miller', 'patient1@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(6, 'Emily Clark', 'patient2@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(7, 'Michael Brown', 'patient3@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(8, 'Sophia Davis', 'patient4@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(9, 'Daniel Wilson', 'patient5@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(10, 'Olivia Johnson', 'patient6@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(11, 'Liam Anderson', 'patient7@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(12, 'Ava Thompson', 'patient8@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(13, 'Noah Martinez', 'patient9@gmail.com', 'patient123', 3, '2025-11-24 19:34:18'),
(14, 'Isabella Roberts', 'patient10@gmail.com', 'patient123', 3, '2025-11-24 19:34:18');

-- ======================================
-- TABLE: clinicians
-- ======================================
CREATE TABLE `clinicians` (
  `clinician_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `speciality` varchar(150) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT '1',
  `phone` varchar(50) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `department` varchar(150) DEFAULT NULL,
  `bio` text,
  PRIMARY KEY (`clinician_id`),
  UNIQUE KEY `user_id` (`user_id`),
  CONSTRAINT `clinicians_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO clinicians (clinician_id, user_id, speciality, is_active, phone, email, department, bio) VALUES
(1, 2, 'Diabetology', 1, NULL, NULL, NULL, NULL),
(2, 3, 'Vascular Surgery', 1, NULL, NULL, NULL, NULL),
(3, 4, 'Podiatry', 1, NULL, NULL, NULL, NULL);

-- ======================================
-- TABLE: patients
-- ======================================
CREATE TABLE `patients` (
  `patient_id` int NOT NULL AUTO_INCREMENT,
  `full_name` varchar(150) NOT NULL,
  `age` int DEFAULT NULL,
  `gender` varchar(20) DEFAULT NULL,
  `contact_number` varchar(50) DEFAULT NULL,
  `risk_level` enum('Low','Medium','High','Critical') DEFAULT 'Low',
  `current_status` varchar(100) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `user_id` int DEFAULT NULL,
  PRIMARY KEY (`patient_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO patients (patient_id, full_name, age, gender, contact_number, risk_level, current_status, created_at, user_id) VALUES
(1, 'John Miller', 58, 'Male', '07000 000022', 'High', 'Under monitoring', '2025-11-24 19:34:18', NULL),
(2, 'Emily Clark', 63, 'Female', '07000 000002', 'Medium', 'Stable', '2025-11-24 19:34:18', NULL),
(3, 'Michael Brown', 51, 'Male', '07000 000003', 'High', 'At risk', '2025-11-24 19:34:18', NULL),
(4, 'Sophia Davis', 47, 'Female', '07000 000004', 'Low', 'Stable', '2025-11-24 19:34:18', NULL),
(5, 'Daniel Wilson', 69, 'Male', '07000 000005', 'High', 'Under monitoring', '2025-11-24 19:34:18', NULL),
(6, 'Olivia Johnson', 56, 'Female', '07000 000006', 'Medium', 'Stable', '2025-11-24 19:34:18', NULL),
(7, 'Liam Anderson', 60, 'Male', '07000 000007', 'High', 'At risk', '2025-11-24 19:34:18', NULL),
(8, 'Ava Thompson', 49, 'Female', '07000 000008', 'Low', 'Stable', '2025-11-24 19:34:18', NULL),
(9, 'Noah Martinez', 65, 'Male', '07000 000009', 'Medium', 'Under monitoring', '2025-11-24 19:34:18', NULL),
(10, 'Isabella Roberts', 53, 'Female', '07000 000010', 'Medium', 'Stable', '2025-11-24 19:34:18', NULL);

-- ======================================
-- TABLE: alerts
-- ======================================
CREATE TABLE `alerts` (
  `alert_id` int NOT NULL AUTO_INCREMENT,
  `patient_id` int NOT NULL,
  `alert_type` varchar(100) NOT NULL,
  `severity` enum('critical','warning','normal','info') DEFAULT NULL,
  `alert_description` text,
  `status` enum('Open','Acknowledged','Resolved') DEFAULT 'Open',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `resolved_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`alert_id`),
  KEY `patient_id` (`patient_id`),
  CONSTRAINT `alerts_ibfk_1` FOREIGN KEY (`patient_id`) REFERENCES `patients` (`patient_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO alerts (alert_id, patient_id, alert_type, severity, alert_description, status, created_at, resolved_at) VALUES
(1, 1, 'Appointment Scheduled', 'info', 'New appointment on Dec 02, 12:12', 'Open', '2025-11-27 14:33:43', NULL),
(2, 1, 'Appointment Scheduled', 'info', 'New appointment on Nov 29, 12:14', 'Open', '2025-11-27 14:34:13', NULL);

-- ======================================
-- TABLE: alert_escalation_history
-- ======================================
CREATE TABLE `alert_escalation_history` (
  `escalation_id` int NOT NULL AUTO_INCREMENT,
  `alert_id` int NOT NULL,
  `from_severity` enum('Low','Medium','High','Critical') DEFAULT NULL,
  `to_severity` enum('Low','Medium','High','Critical') DEFAULT NULL,
  `escalated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `comments` text,
  PRIMARY KEY (`escalation_id`),
  KEY `alert_id` (`alert_id`),
  CONSTRAINT `alert_escalation_history_ibfk_1` FOREIGN KEY (`alert_id`) REFERENCES `alerts` (`alert_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- ======================================
-- TABLE: clinician_patient_assignment
-- ======================================
CREATE TABLE `clinician_patient_assignment` (
  `assignment_id` int NOT NULL AUTO_INCREMENT,
  `patient_id` int NOT NULL,
  `clinician_id` int NOT NULL,
  `is_primary` tinyint(1) DEFAULT '0',
  `assigned_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`assignment_id`),
  UNIQUE KEY `unique_primary_clinician` (`patient_id`,`is_primary`),
  KEY `clinician_id` (`clinician_id`),
  CONSTRAINT `clinician_patient_assignment_ibfk_1` FOREIGN KEY (`patient_id`) REFERENCES `patients` (`patient_id`),
  CONSTRAINT `clinician_patient_assignment_ibfk_2` FOREIGN KEY (`clinician_id`) REFERENCES `clinicians` (`clinician_id`),
  CONSTRAINT `chk_primary_boolean` CHECK ((`is_primary` in (0,1)))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO clinician_patient_assignment (assignment_id, patient_id, clinician_id, is_primary, assigned_at) VALUES
(1, 1, 1, 1, '2025-11-24 19:34:18'),
(2, 2, 1, 1, '2025-11-24 19:34:18'),
(3, 3, 1, 1, '2025-11-24 19:34:18'),
(4, 4, 2, 1, '2025-11-24 19:34:18'),
(5, 5, 2, 1, '2025-11-24 19:34:18'),
(6, 6, 2, 1, '2025-11-24 19:34:18'),
(7, 7, 3, 1, '2025-11-24 19:34:18'),
(8, 8, 3, 1, '2025-11-24 19:34:18'),
(9, 9, 3, 1, '2025-11-24 19:34:18'),
(10, 10, 1, 1, '2025-11-24 19:34:18');

-- ======================================
-- TABLE: appointments
-- ======================================
CREATE TABLE `appointments` (
  `appointment_id` int NOT NULL AUTO_INCREMENT,
  `patient_id` int NOT NULL,
  `clinician_id` int NOT NULL,
  `appointment_time` datetime NOT NULL,
  `status` varchar(50) NOT NULL DEFAULT 'Scheduled',
  `notes` text,
  PRIMARY KEY (`appointment_id`),
  KEY `fk_appt_patient` (`patient_id`),
  KEY `fk_appt_clinician` (`clinician_id`),
  CONSTRAINT `fk_appt_patient` FOREIGN KEY (`patient_id`) REFERENCES `patients` (`patient_id`),
  CONSTRAINT `fk_appt_clinician` FOREIGN KEY (`clinician_id`) REFERENCES `clinicians` (`clinician_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO appointments (appointment_id, patient_id, clinician_id, appointment_time, status, notes) VALUES
(1, 1, 1, '2025-12-02 12:12:00', 'Scheduled', 'general check up'),
(2, 1, 1, '2025-12-02 12:12:00', 'Scheduled', 'general check up'),
(3, 1, 1, '2025-12-02 12:12:00', 'Scheduled', 'general check up'),
(4, 1, 1, '2025-11-29 12:14:00', 'Scheduled', 'check');

-- ======================================
-- TABLE: clinical_notes
-- ======================================
CREATE TABLE `clinical_notes` (
  `note_id` int NOT NULL AUTO_INCREMENT,
  `patient_id` int NOT NULL,
  `clinician_id` int NOT NULL,
  `content` text NOT NULL,
  `created_at` datetime NOT NULL,
  PRIMARY KEY (`note_id`),
  KEY `fk_note_patient` (`patient_id`),
  KEY `fk_note_clinician` (`clinician_id`),
  CONSTRAINT `fk_note_patient` FOREIGN KEY (`patient_id`) REFERENCES `patients` (`patient_id`),
  CONSTRAINT `fk_note_clinician` FOREIGN KEY (`clinician_id`) REFERENCES `clinicians` (`clinician_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- ======================================
-- TABLE: audit_log
-- ======================================
CREATE TABLE `audit_log` (
  `audit_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int DEFAULT NULL,
  `action_type` varchar(100) DEFAULT NULL,
  `table_name` varchar(100) DEFAULT NULL,
  `record_id` int DEFAULT NULL,
  `action_timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `details` text,
  PRIMARY KEY (`audit_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `audit_log_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- ======================================
-- TABLE: auth_metadata
-- ======================================
CREATE TABLE `auth_metadata` (
  `meta_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `last_login` timestamp NULL DEFAULT NULL,
  `failed_attempts` int DEFAULT '0',
  `password_updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`meta_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `auth_metadata_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- ======================================
-- TABLE: notifications
-- ======================================
CREATE TABLE `notifications` (
  `notification_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `alert_id` int DEFAULT NULL,
  `message` text NOT NULL,
  `delivered` tinyint(1) DEFAULT '0',
  `delivered_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`notification_id`),
  KEY `user_id` (`user_id`),
  KEY `alert_id` (`alert_id`),
  CONSTRAINT `notifications_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`),
  CONSTRAINT `notifications_ibfk_2` FOREIGN KEY (`alert_id`) REFERENCES `alerts` (`alert_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- ======================================
-- TABLE: patient_messages
-- ======================================
CREATE TABLE `patient_messages` (
  `message_id` int NOT NULL AUTO_INCREMENT,
  `patient_id` int NOT NULL,
  `clinician_id` int DEFAULT NULL,
  `from_clinician` tinyint(1) NOT NULL,
  `content` text NOT NULL,
  `created_at` datetime NOT NULL,
  PRIMARY KEY (`message_id`),
  KEY `patient_id` (`patient_id`),
  KEY `clinician_id` (`clinician_id`),
  CONSTRAINT `patient_messages_ibfk_1` FOREIGN KEY (`patient_id`) REFERENCES `patients` (`patient_id`),
  CONSTRAINT `patient_messages_ibfk_2` FOREIGN KEY (`clinician_id`) REFERENCES `clinicians` (`clinician_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO patient_messages (message_id, patient_id, clinician_id, from_clinician, content, created_at) VALUES
(1, 1, 1, 0, 'hello', '2025-11-24 20:11:36'),
(2, 1, 1, 0, 'hello', '2025-11-25 11:53:51'),
(3, 1, 1, 1, 'yes?', '2025-11-25 14:35:38');

-- ======================================
-- TABLE: patient_risk_history
-- ======================================
CREATE TABLE `patient_risk_history` (
  `history_id` int NOT NULL AUTO_INCREMENT,
  `patient_id` int NOT NULL,
  `old_risk` enum('Low','Medium','High','Critical') DEFAULT NULL,
  `new_risk` enum('Low','Medium','High','Critical') DEFAULT NULL,
  `changed_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`history_id`),
  KEY `patient_id` (`patient_id`),
  CONSTRAINT `patient_risk_history_ibfk_1` FOREIGN KEY (`patient_id`) REFERENCES `patients` (`patient_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- ======================================
-- TABLE: sensor_readings
-- ======================================
CREATE TABLE `sensor_readings` (
  `reading_id` int NOT NULL AUTO_INCREMENT,
  `patient_id` int NOT NULL,
  `timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `value` text,
  PRIMARY KEY (`reading_id`),
  KEY `patient_id` (`patient_id`),
  CONSTRAINT `sensor_readings_ibfk_1` FOREIGN KEY (`patient_id`) REFERENCES `patients` (`patient_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

