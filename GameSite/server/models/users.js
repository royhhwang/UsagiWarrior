module.exports = function(sequelize, DataTypes) {
  const Users = sequelize.define("Users", {
    username: {
      type: DataTypes.STRING(50),
      allowNull: false,
      validate: {
        len: [1, 50]
      }
    },
    password: {
      type: DataTypes.STRING(150),
      allowNull: false,
      validate: {
        len: [1, 150]
      }
    }
  });
  return Users;
};
