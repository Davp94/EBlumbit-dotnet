BEGIN;

DO $$
BEGIN
    IF (SELECT COUNT(*) FROM roles WHERE id IN (1, 2, 3)) <> 3 THEN
        RAISE EXCEPTION 'Expected roles 1, 2, and 3 to exist';
    END IF;

    IF (SELECT COUNT(*) FROM users WHERE id BETWEEN 1 AND 10) <> 10 THEN
        RAISE EXCEPTION 'Expected users 1 through 10 to exist';
    END IF;
END
$$;

-- Replace role assignments for users 1 through 10 with the requested mapping.
DELETE FROM role_user
WHERE user_id BETWEEN 1 AND 10;

INSERT INTO role_user (role_id, user_id)
VALUES
    (1, 1),
    (1, 2),
    (1, 3),
    (1, 4),
    (2, 5),
    (2, 6),
    (2, 7),
    (2, 8),
    (3, 9),
    (3, 10);

-- Give the admin role every permission currently in the database.
INSERT INTO permission_role (role_id, permission_id)
SELECT 1, p.id
FROM permissions AS p
WHERE NOT EXISTS (
    SELECT 1
    FROM permission_role AS pr
    WHERE pr.role_id = 1
      AND pr.permission_id = p.id
);

COMMIT;
