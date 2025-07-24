import React, { useState, useEffect } from 'react';
import { Modal, Box, TextField, Button } from '@mui/material';
import './WellFormModal.css';
import {axiosToBackend} from "../../hooks/useAxios.js";
import {toast} from "react-toastify";

const style = {
    position: 'absolute', top: '50%', left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400, bgcolor: 'background.paper',
    boxShadow: 24, p: 4, borderRadius: 2
};

const WellFormModal = ({ open, handleClose, refreshData, editData }) => {
    const [formData, setFormData] = useState({
        wellNumber: '',
        debit: '',
        pressure: '',
        measurementDate: '',
        fieldId: ''
    });

    useEffect(() => {
        if (editData) setFormData(editData);
        else setFormData({ wellNumber: '', debit: '', pressure: '', measurementDate: '', fieldId: '' });
    }, [editData]);

    const handleChange = (e) => {
        setFormData(prev => ({ ...prev, [e.target.name]: e.target.value }));
    };

    const handleSubmit = async () => {
        if (editData?.wellId) {
            await axiosToBackend.put(`/wells/${editData.wellId}`, formData)
                .then(() => toast.success("Well successfully updated"))
                .catch(e => {
                    switch (e.status) {
                        case 400:
                            toast.error("400: Incorrect input", {toastId: `UpdateError ${editData.wellId}`});
                            break;
                        case 404:
                            toast.error("404: Well not found", {toastId: `UpdateError ${editData.wellId}`})
                            break;
                        case 500:
                            toast.error("505: Internal Server Error", {toastId: `UpdateError ${editData.wellId}`})
                    }
                } );
        } else {
            await axiosToBackend.post('/wells', formData)
                .then(() => toast.success("Well successfully created"))
                .catch(e => {
                    switch (e.status) {
                        case 400:
                            toast.error("400: Incorrect input", {toastId: 'CreateError'});
                            break;
                        case 500:
                            toast.error("505: Internal Server Error", {toastId: 'CreateError'})
                    }
                });
        }
        handleClose();
        refreshData();
    };

    return (
        <Modal open={open} onClose={handleClose}>
            <Box sx={style}>
                <TextField style={{boxShadow: "none"}} label="Номер скважины" name="wellNumber" value={formData.wellNumber} onChange={handleChange} fullWidth margin="normal" />
                <TextField label="Дебит" name="debit" type="number" value={formData.debit} onChange={handleChange} fullWidth margin="normal" />
                <TextField label="Давление" name="pressure" type="number" value={formData.pressure} onChange={handleChange} fullWidth margin="normal" />
                <TextField label="Дата замера" name="measurementDate" type="date" value={formData.measurementDate} onChange={handleChange} fullWidth margin="normal" />
                <TextField label="ID месторождения" name="fieldId" type="number" value={formData.fieldId} onChange={handleChange} fullWidth margin="normal" />
                <Button variant="contained" onClick={handleSubmit} fullWidth>
                    {editData ? 'Обновить' : 'Создать'}
                </Button>
            </Box>
        </Modal>
    );
};

export default WellFormModal;
