create sequence users_id start 1;
create table karyawan(id_karyawan character varying(100) default '0' || nextval('users_id') primary key,
					  nama character varying(20),
					  id_dep character varying(20));

create table departemen(
	id_dep character varying(100) primary key,
	nama_dep character varying(100));

INSERT INTO departemen(id_dep, nama_dep) values ('HR','HR'),('ENG','Engineer'),('DEV','Developer'),('PM','Product M'),('FIN','Finance');


CREATE OR REPLACE function st_insert
(
_nama character varying,
_id_dep character varying
) 
returns int AS
'
BEGIN
	insert into public.karyawan
	(
		nama,
		id_dep
	)
	values
	(
		_nama, 
		_id_dep
	);
	if found then
 		return 1;
	else 
		return 0;
	end if;
end
'
language plpgsql;


CREATE OR REPLACE function st_select()
returns table
(
	_id_karyawan character varying,
	_nama character varying,
	_id_dep character varying,
	_nama_dep character varying
)
language plpgsql
as
'
begin
return query
select karyawan.id_karyawan, karyawan.nama, karyawan.id_dep, departemen.nama_dep from karyawan INNER JOIN departemen ON karyawan.id_dep = departemen.id_dep;
end
'


CREATE OR REPLACE function st_update
(
	_id_karyawan character varying,
	_nama character varying,
	_id_dep character varying
)
returns int
as
'
BEGIN
	update karyawan SET
		nama = _nama,
		id_dep = _id_dep
	where id_karyawan = _id_karyawan;
	if found then
		return 1;
	else
		return 0;
	end if;
end
'
language plpgsql


CREATE OR REPLACE FUNCTION st_delete(_id_karyawan character varying)
returns int AS
'
begin
	delete from public.karyawan where id_karyawan = _id_karyawan;
	if found then
		return 1;
	else return 0;
	end if;
end
'
language plpgsql